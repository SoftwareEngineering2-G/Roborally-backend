#!/bin/sh

SOURCE_DIR="src"
AUTHOR_MAP="authors.map"
IGNORE_REVS=".git-blame-ignore-revs"
THRESHOLD=20

BLAME_OPTS="--line-porcelain"
[ -f "$IGNORE_REVS" ] && BLAME_OPTS="$BLAME_OPTS --ignore-revs-file $IGNORE_REVS"

echo "Adding per-method multi-author tags..."

git ls-files "$SOURCE_DIR" | grep "\.cs$" | while read -r file; do
  [ ! -s "$file" ] && continue

  tmp=$(mktemp)

  awk -v file="$file" \
      -v blame_opts="$BLAME_OPTS" \
      -v map="$AUTHOR_MAP" \
      -v threshold="$THRESHOLD" '

  function blame_authors(start, end,   cmd, line, a, total, out) {
    cmd = "git blame " blame_opts " -L " start "," end " \"" file "\" 2>/dev/null | grep \"^author \""
    total = 0
    while ((cmd | getline line) > 0) {
      sub("^author ", "", line)
      count[line]++
      total++
    }
    close(cmd)

    out = ""
    for (a in count) {
      if ((count[a] / total) * 100 >= threshold) {
        name = a
        if (map != "") {
          cmd2 = "grep \"^" a "[[:space:]]*=\" " map " | cut -d= -f2 | xargs"
          cmd2 | getline mapped
          close(cmd2)
          if (mapped != "") name = mapped
        }
        out = out "/// <author name=\"" name "\" />\n"
      }
    }

    delete count
    return out
  }

  BEGIN {
    depth = 0
    in_method = 0
    start_line = 0
    author_block = ""
  }

  # Detect method signature
  /^[[:space:]]*(public|protected|internal)[^;]+\(.*\)[[:space:]]*\{/ {
    in_method = 1
    start_line = NR
    depth = 1
  }

  {
    if (in_method) {
      depth += gsub(/\{/, "{")
      depth -= gsub(/\}/, "}")
      if (depth == 0) {
        end_line = NR
        author_block = blame_authors(start_line, end_line)
        in_method = 0
      }
    }
  }

  # Insert after </summary>
  /<\/summary>/ && author_block != "" {
    print
    printf "%s", author_block
    author_block = ""
    next
  }

  # Insert above method if no XML docs
  /^[[:space:]]*(public|protected|internal)/ && author_block != "" {
    printf "%s", author_block
    author_block = ""
  }

  { print }
  ' "$file" > "$tmp"

  mv "$tmp" "$file"
  echo "Processed: $file"
done

echo "Done. Review changes carefully."
