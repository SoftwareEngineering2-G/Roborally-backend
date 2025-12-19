import subprocess
import re
from pathlib import Path

SRC = Path("src")
AUTHOR_MAP = {}

if Path("authors.map").exists():
    for line in Path("authors.map").read_text().splitlines():
        if "=" in line:
            k, v = line.split("=", 1)
            AUTHOR_MAP[k.strip()] = v.strip()

access_modifiers = ["public", "protected", "internal", "private"]

for file in SRC.rglob("*.cs"):
    lines = file.read_text().splitlines()
    new_lines = []

    for i, line in enumerate(lines):
        stripped = line.strip()

        # Skip existing author tags
        if "<author" in stripped:
            new_lines.append(line)
            continue

        # Detect access modifiers
        if any(stripped.startswith(mod) for mod in access_modifiers):
            # Detect method by presence of parentheses
            if "(" in stripped and ")" in stripped:
                # git blame for this line
                try:
                    out = subprocess.check_output(
                        ["git", "blame", "-L", f"{i+1},{i+1}", str(file)],
                        text=True,
                        stderr=subprocess.DEVNULL,
                    )
                    author = out.split("(")[1].split(")")[0].strip()
                    author = AUTHOR_MAP.get(author, author)
                    new_lines.append(f'/// <author name="{author}" />')
                except Exception:
                    pass

        new_lines.append(line)

    file.write_text("\n".join(new_lines))
    print(f"Processed {file}")
