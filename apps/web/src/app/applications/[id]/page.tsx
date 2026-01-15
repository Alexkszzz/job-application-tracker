"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import Link from "next/link";
import { Application, ApplicationStatus } from "@/lib/api/types";
import { applicationService } from "@/lib/api/applications";
import ProtectedRoute from "@/components/ProtectedRoute";

const StatusBadge = ({ status }: { status: ApplicationStatus }) => {
  const styles = {
    [ApplicationStatus.Applied]: "bg-blue-100 text-blue-800",
    [ApplicationStatus.Interview]: "bg-yellow-100 text-yellow-800",
    [ApplicationStatus.Offer]: "bg-green-100 text-green-800",
    [ApplicationStatus.Rejected]: "bg-red-100 text-red-800",
  };

  const labels = {
    [ApplicationStatus.Applied]: "Applied",
    [ApplicationStatus.Interview]: "Interview",
    [ApplicationStatus.Offer]: "Offer",
    [ApplicationStatus.Rejected]: "Rejected",
  };

  return (
    <span
      className={`px-3 py-1 rounded-full text-sm font-medium ${styles[status]}`}
    >
      {labels[status]}
    </span>
  );
};

export default function ApplicationDetailPage() {
  const params = useParams();
  const router = useRouter();
  const [application, setApplication] = useState<Application | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [deleting, setDeleting] = useState(false);

  useEffect(() => {
    const fetchApplication = async () => {
      try {
        setLoading(true);
        const data = await applicationService.getById(params.id as string);
        setApplication(data);
      } catch (err) {
        setError("Failed to load application");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    if (params.id) {
      fetchApplication();
    }
  }, [params.id]);

  const handleDelete = async () => {
    if (
      !application ||
      !confirm("Are you sure you want to delete this application?")
    ) {
      return;
    }

    try {
      setDeleting(true);
      await applicationService.delete(application.id);
      router.push("/applications");
    } catch (err) {
      setError("Failed to delete application");
      console.error(err);
      setDeleting(false);
    }
  };

  if (loading) {
    return (
      <ProtectedRoute>
        <div className="flex justify-center items-center min-h-[400px]">
          <div className="text-lg">Loading...</div>
        </div>
      </ProtectedRoute>
    );
  }

  if (error || !application) {
    return (
      <ProtectedRoute>
        <div className="max-w-4xl mx-auto px-4 py-8">
          <div className="bg-red-50 border border-red-200 rounded-lg p-4 text-red-800">
            {error || "Application not found"}
          </div>
          <Link
            href="/applications"
            className="mt-4 inline-block text-blue-600 hover:underline"
          >
            ← Back to Applications
          </Link>
        </div>
      </ProtectedRoute>
    );
  }

  return (
    <ProtectedRoute>
      <div className="max-w-4xl mx-auto px-4 py-8">
        {/* Header */}
        <div className="mb-6">
          <Link
            href="/applications"
            className="text-blue-600 hover:underline mb-4 inline-block"
          >
            ← Back to Applications
          </Link>
          <div className="flex justify-between items-start">
            <div>
              <h1 className="text-3xl font-bold mb-2">
                {application.jobTitle}
              </h1>
              <p className="text-xl text-gray-600">{application.companyName}</p>
            </div>
            <StatusBadge status={application.status} />
          </div>
        </div>

        {/* Actions */}
        <div className="flex gap-3 mb-8">
          <Link
            href={`/applications/${application.id}/edit`}
            className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            Edit Application
          </Link>
          <button
            onClick={handleDelete}
            disabled={deleting}
            className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors disabled:opacity-50"
          >
            {deleting ? "Deleting..." : "Delete"}
          </button>
        </div>

        {/* Details Card */}
        <div className="bg-white rounded-lg shadow-md p-6 space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Applied Date */}
            <div>
              <h3 className="text-sm font-medium text-gray-500 mb-1">
                Applied Date
              </h3>
              <p className="text-lg">
                {new Date(application.appliedDate).toLocaleDateString()}
              </p>
            </div>

            {/* Interview Date */}
            {application.interviewDate && (
              <div>
                <h3 className="text-sm font-medium text-gray-500 mb-1">
                  Interview Date
                </h3>
                <p className="text-lg">
                  {new Date(application.interviewDate).toLocaleDateString()}
                </p>
              </div>
            )}

            {/* Location */}
            {application.location && (
              <div>
                <h3 className="text-sm font-medium text-gray-500 mb-1">
                  Location
                </h3>
                <p className="text-lg">{application.location}</p>
              </div>
            )}

            {/* Salary */}
            {application.salary && (
              <div>
                <h3 className="text-sm font-medium text-gray-500 mb-1">
                  Salary
                </h3>
                <p className="text-lg">
                  ${application.salary.toLocaleString()}
                </p>
              </div>
            )}
          </div>

          {/* Job URL */}
          {application.jobUrl && (
            <div>
              <h3 className="text-sm font-medium text-gray-500 mb-1">
                Job Posting
              </h3>
              <a
                href={application.jobUrl}
                target="_blank"
                rel="noopener noreferrer"
                className="text-blue-600 hover:underline break-all"
              >
                {application.jobUrl}
              </a>
            </div>
          )}

          {/* Job Description */}
          <div>
            <h3 className="text-sm font-medium text-gray-500 mb-1">
              Job Description
            </h3>
            <p className="text-gray-800 whitespace-pre-wrap">
              {application.jobDescription}
            </p>
          </div>

          {/* Notes */}
          {application.notes && (
            <div>
              <h3 className="text-sm font-medium text-gray-500 mb-1">Notes</h3>
              <p className="text-gray-800 whitespace-pre-wrap">
                {application.notes}
              </p>
            </div>
          )}

          {/* Documents */}
          <div>
            <h3 className="text-sm font-medium text-gray-500 mb-2">
              Documents
            </h3>
            <div className="flex flex-wrap gap-3">
              {application.resumeUrl ? (
                <a
                  href={application.resumeUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="flex items-center gap-2 px-4 py-2 bg-blue-50 text-blue-700 rounded-lg hover:bg-blue-100 transition-colors"
                >
                  <svg
                    className="w-5 h-5"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
                    />
                  </svg>
                  View Resume
                </a>
              ) : (
                <div className="px-4 py-2 bg-gray-50 text-gray-400 rounded-lg">
                  <span className="text-sm">No resume uploaded</span>
                </div>
              )}

              {application.coverLetterUrl ? (
                <a
                  href={application.coverLetterUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="flex items-center gap-2 px-4 py-2 bg-green-50 text-green-700 rounded-lg hover:bg-green-100 transition-colors"
                >
                  <svg
                    className="w-5 h-5"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
                    />
                  </svg>
                  View Cover Letter
                </a>
              ) : (
                <div className="px-4 py-2 bg-gray-50 text-gray-400 rounded-lg">
                  <span className="text-sm">No cover letter uploaded</span>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </ProtectedRoute>
  );
}
