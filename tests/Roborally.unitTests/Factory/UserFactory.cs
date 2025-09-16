namespace Roborally.unitTests.Factory;

public static class UserFactory {

    public static IEnumerable<object[]> GetValidUsernames() {
        return new List<object[]>() {
            new object[]{"Alice"},
            new object[]{"Bob123"},
            new object[]{"ab"}, // Min
            new object[]{"a".PadRight(100, 'a')},// Max
        } ;
    }

    public static IEnumerable<object[]> GetInvalidUsernames() {
        return new List<object[]>() {
            new object[]{""}, // Empty
            new object[]{"a"}, // Too short
            new object[]{"a".PadRight(101, 'a')}, // Too long
            new object[]{"   "}, // Only spaces
        } ;
    }

    public static IEnumerable<object[]> GetValidPasswords() {
        return new List<object[]>() {
            new object[]{"pass"},
            new object[]{"password123"},
            new object[]{"1234"}, // Min
            new object[]{"p".PadRight(100, 'p')}, // Max
            new object[]{"P@ssw0rd!"},
            new object[]{"MySecurePass123!"}
        };
    }

    public static IEnumerable<object[]> GetInvalidPasswords() {
        return new List<object[]>() {
            new object[]{""}, // Empty
            new object[]{"123"}, // Too short
            new object[]{"p".PadRight(101, 'p')}, // Too long
            new object[]{"   "}, // Only spaces (less than 4 chars)
        };
    }
    
}