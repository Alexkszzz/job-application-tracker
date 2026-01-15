"use client";

import Link from "next/link";
import { useAuth } from "@/contexts/AuthContext";
import { usePathname } from "next/navigation";

export default function Navigation() {
  const { user, logout, isAuthenticated } = useAuth();
  const pathname = usePathname();

  // Don't show navigation on login/register pages
  const authPages = ["/login", "/register"];
  if (authPages.includes(pathname)) {
    return null;
  }

  return (
    <header className="bg-white shadow">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
        <div className="flex justify-between items-center">
          <Link href="/" className="text-2xl font-bold text-gray-900">
            Job Tracker
          </Link>
          {isAuthenticated ? (
            <nav className="flex gap-6 items-center">
              <Link href="/" className="text-gray-600 hover:text-gray-900">
                Dashboard
              </Link>
              <Link
                href="/applications"
                className="text-gray-600 hover:text-gray-900"
              >
                Applications
              </Link>
              <Link
                href="/applications/new"
                className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700"
              >
                + New Application
              </Link>
              <div className="flex items-center gap-4 border-l pl-6 ml-2">
                <span className="text-sm text-gray-600">
                  {user?.firstName || user?.email}
                </span>
                <button
                  onClick={logout}
                  className="text-sm text-gray-600 hover:text-gray-900"
                >
                  Logout
                </button>
              </div>
            </nav>
          ) : (
            <nav className="flex gap-4">
              <Link
                href="/login"
                className="text-gray-600 hover:text-gray-900 px-4 py-2"
              >
                Login
              </Link>
              <Link
                href="/register"
                className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700"
              >
                Sign Up
              </Link>
            </nav>
          )}
        </div>
      </div>
    </header>
  );
}
