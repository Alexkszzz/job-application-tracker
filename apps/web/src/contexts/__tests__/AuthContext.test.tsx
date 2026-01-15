import { render, screen, waitFor } from "@testing-library/react";
import { useRouter } from "next/navigation";
import { AuthProvider, useAuth } from "@/contexts/AuthContext";
import "@testing-library/jest-dom";

// Mock next/navigation
jest.mock("next/navigation", () => ({
  useRouter: jest.fn(),
  usePathname: jest.fn(),
}));

// Mock localStorage
const localStorageMock = (() => {
  let store: Record<string, string> = {};
  return {
    getItem: (key: string) => store[key] || null,
    setItem: (key: string, value: string) => {
      store[key] = value.toString();
    },
    removeItem: (key: string) => {
      delete store[key];
    },
    clear: () => {
      store = {};
    },
  };
})();

Object.defineProperty(window, "localStorage", {
  value: localStorageMock,
});

// Test component to access auth context
function TestComponent() {
  const { user, isAuthenticated, isLoading } = useAuth();
  return (
    <div>
      <div data-testid="loading">{isLoading ? "loading" : "not-loading"}</div>
      <div data-testid="authenticated">
        {isAuthenticated ? "authenticated" : "not-authenticated"}
      </div>
      {user && <div data-testid="user-email">{user.email}</div>}
    </div>
  );
}

describe("AuthContext", () => {
  const mockRouter = {
    push: jest.fn(),
    replace: jest.fn(),
    back: jest.fn(),
    forward: jest.fn(),
    refresh: jest.fn(),
    prefetch: jest.fn(),
  };

  beforeEach(() => {
    (useRouter as jest.Mock).mockReturnValue(mockRouter);
    localStorage.clear();
    jest.clearAllMocks();
  });

  it("should initialize with loading state", () => {
    render(
      <AuthProvider>
        <TestComponent />
      </AuthProvider>
    );

    expect(screen.getByTestId("loading")).toHaveTextContent("loading");
  });

  it("should show not authenticated when no token exists", async () => {
    render(
      <AuthProvider>
        <TestComponent />
      </AuthProvider>
    );

    await waitFor(() => {
      expect(screen.getByTestId("loading")).toHaveTextContent("not-loading");
    });

    expect(screen.getByTestId("authenticated")).toHaveTextContent(
      "not-authenticated"
    );
  });

  it("should load user from localStorage with valid token", async () => {
    const mockUser = {
      userId: "123",
      email: "test@example.com",
      firstName: "Test",
      lastName: "User",
    };

    // Create a mock JWT token with expiration in the future
    const futureTimestamp = Math.floor(Date.now() / 1000) + 3600; // 1 hour from now
    const mockJwtPayload = {
      sub: mockUser.userId,
      email: mockUser.email,
      exp: futureTimestamp,
    };
    const mockToken = `header.${btoa(
      JSON.stringify(mockJwtPayload)
    )}.signature`;

    localStorage.setItem("token", mockToken);
    localStorage.setItem("user", JSON.stringify(mockUser));

    render(
      <AuthProvider>
        <TestComponent />
      </AuthProvider>
    );

    await waitFor(() => {
      expect(screen.getByTestId("authenticated")).toHaveTextContent(
        "authenticated"
      );
    });

    expect(screen.getByTestId("user-email")).toHaveTextContent(
      "test@example.com"
    );
  });

  it("should clear expired token on load", async () => {
    const mockUser = {
      userId: "123",
      email: "test@example.com",
    };

    // Create a mock JWT token that expired in the past
    const pastTimestamp = Math.floor(Date.now() / 1000) - 3600; // 1 hour ago
    const mockJwtPayload = {
      sub: mockUser.userId,
      email: mockUser.email,
      exp: pastTimestamp,
    };
    const expiredToken = `header.${btoa(
      JSON.stringify(mockJwtPayload)
    )}.signature`;

    localStorage.setItem("token", expiredToken);
    localStorage.setItem("user", JSON.stringify(mockUser));

    render(
      <AuthProvider>
        <TestComponent />
      </AuthProvider>
    );

    await waitFor(() => {
      expect(screen.getByTestId("authenticated")).toHaveTextContent(
        "not-authenticated"
      );
    });

    expect(localStorage.getItem("token")).toBeNull();
    expect(localStorage.getItem("user")).toBeNull();
  });
});
