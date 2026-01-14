"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { ApplicationStatus, CreateApplicationRequest } from "@/lib/api/types";
import { applicationService } from "@/lib/api/applications";
import Toast from "@/components/Toast";

export default function NewApplicationPage() {
  const router = useRouter();
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [toast, setToast] = useState<{
    message: string;
    type: "success" | "error";
  } | null>(null);

  const [formData, setFormData] = useState<CreateApplicationRequest>({
    companyName: "",
    jobTitle: "",
    status: ApplicationStatus.Applied,
    jobDescription: "",
    location: "",
    salary: undefined,
    appliedDate: new Date().toISOString().split("T")[0], // Default to today
    interviewDate: "",
    notes: "",
    jobUrl: "",
    resumeUrl: "",
    coverLetterUrl: "",
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    setError(null);

    try {
      // Clean up empty strings to undefined
      const createData: CreateApplicationRequest = {
        companyName: formData.companyName,
        jobTitle: formData.jobTitle,
        status: formData.status,
        jobDescription: formData.jobDescription,
        appliedDate: formData.appliedDate,
        location: formData.location || undefined,
        salary: formData.salary || undefined,
        interviewDate: formData.interviewDate || undefined,
        notes: formData.notes || undefined,
        jobUrl: formData.jobUrl || undefined,
        resumeUrl: formData.resumeUrl || undefined,
        coverLetterUrl: formData.coverLetterUrl || undefined,
      };

      const newApp = await applicationService.create(createData);
      setToast({
        message: "Application created successfully!",
        type: "success",
      });

      // Redirect after showing toast
      setTimeout(() => {
        router.push(`/applications/${newApp.id}`);
      }, 1500);
    } catch (err) {
      setError("Failed to create application");
      setToast({ message: "Failed to create application", type: "error" });
      console.error(err);
      setSubmitting(false);
    }
  };

  return (
    <div className="max-w-2xl mx-auto px-4 py-8">
      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}

      <div className="mb-6">
        <Link
          href="/applications"
          className="text-blue-600 hover:underline mb-4 inline-block"
        >
          ← Back to Applications
        </Link>
        <h1 className="text-3xl font-bold">New Application</h1>
      </div>

      {error && (
        <div className="mb-6 bg-red-50 border border-red-200 rounded-lg p-4 text-red-800">
          {error}
        </div>
      )}

      <form
        onSubmit={handleSubmit}
        className="bg-white rounded-lg shadow-md p-6 space-y-6"
      >
        {/* Company Name */}
        <div>
          <label
            htmlFor="companyName"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Company Name *
          </label>
          <input
            type="text"
            id="companyName"
            required
            value={formData.companyName}
            onChange={(e) =>
              setFormData({ ...formData, companyName: e.target.value })
            }
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="e.g. Microsoft"
          />
        </div>

        {/* Job Title */}
        <div>
          <label
            htmlFor="jobTitle"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Job Title *
          </label>
          <input
            type="text"
            id="jobTitle"
            required
            value={formData.jobTitle}
            onChange={(e) =>
              setFormData({ ...formData, jobTitle: e.target.value })
            }
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="e.g. Senior Software Engineer"
          />
        </div>

        {/* Status */}
        <div>
          <label
            htmlFor="status"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Status *
          </label>
          <select
            id="status"
            required
            value={formData.status}
            onChange={(e) =>
              setFormData({
                ...formData,
                status: parseInt(e.target.value) as ApplicationStatus,
              })
            }
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <option value={ApplicationStatus.Applied}>Applied</option>
            <option value={ApplicationStatus.Interview}>Interview</option>
            <option value={ApplicationStatus.Offer}>Offer</option>
            <option value={ApplicationStatus.Rejected}>Rejected</option>
          </select>
        </div>

        {/* Applied Date */}
        <div>
          <label
            htmlFor="appliedDate"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Applied Date *
          </label>
          <input
            type="date"
            id="appliedDate"
            required
            value={formData.appliedDate}
            onChange={(e) =>
              setFormData({ ...formData, appliedDate: e.target.value })
            }
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Interview Date */}
        <div>
          <label
            htmlFor="interviewDate"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Interview Date
          </label>
          <input
            type="date"
            id="interviewDate"
            value={formData.interviewDate}
            onChange={(e) =>
              setFormData({ ...formData, interviewDate: e.target.value })
            }
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Location */}
        <div>
          <label
            htmlFor="location"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Location
          </label>
          <input
            type="text"
            id="location"
            value={formData.location}
            onChange={(e) =>
              setFormData({ ...formData, location: e.target.value })
            }
            placeholder="e.g. Remote, New York, NY"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Salary */}
        <div>
          <label
            htmlFor="salary"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Salary
          </label>
          <input
            type="number"
            id="salary"
            value={formData.salary || ""}
            onChange={(e) =>
              setFormData({
                ...formData,
                salary: e.target.value ? parseFloat(e.target.value) : undefined,
              })
            }
            placeholder="e.g. 120000"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Job URL */}
        <div>
          <label
            htmlFor="jobUrl"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Job Posting URL
          </label>
          <input
            type="url"
            id="jobUrl"
            value={formData.jobUrl}
            onChange={(e) =>
              setFormData({ ...formData, jobUrl: e.target.value })
            }
            placeholder="https://..."
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Job Description */}
        <div>
          <label
            htmlFor="jobDescription"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Job Description *
          </label>
          <textarea
            id="jobDescription"
            required
            value={formData.jobDescription}
            onChange={(e) =>
              setFormData({ ...formData, jobDescription: e.target.value })
            }
            rows={6}
            placeholder="Paste the job description here..."
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Notes */}
        <div>
          <label
            htmlFor="notes"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Notes
          </label>
          <textarea
            id="notes"
            value={formData.notes}
            onChange={(e) =>
              setFormData({ ...formData, notes: e.target.value })
            }
            rows={4}
            placeholder="Any additional notes..."
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Documents Section */}
        <div className="border-t pt-6">
          <h3 className="text-lg font-medium text-gray-900 mb-4">Documents</h3>

          {/* Resume URL */}
          <div className="mb-4">
            <label
              htmlFor="resumeUrl"
              className="block text-sm font-medium text-gray-700 mb-1"
            >
              Resume URL
            </label>
            <input
              type="url"
              id="resumeUrl"
              value={formData.resumeUrl}
              onChange={(e) =>
                setFormData({ ...formData, resumeUrl: e.target.value })
              }
              placeholder="https://drive.google.com/... or https://..."
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
            <p className="mt-1 text-xs text-gray-500">
              Link to your resume (Google Drive, Dropbox, etc.)
            </p>
          </div>

          {/* Cover Letter URL */}
          <div>
            <label
              htmlFor="coverLetterUrl"
              className="block text-sm font-medium text-gray-700 mb-1"
            >
              Cover Letter URL
            </label>
            <input
              type="url"
              id="coverLetterUrl"
              value={formData.coverLetterUrl}
              onChange={(e) =>
                setFormData({ ...formData, coverLetterUrl: e.target.value })
              }
              placeholder="https://drive.google.com/... or https://..."
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
            <p className="mt-1 text-xs text-gray-500">
              Link to your cover letter (Google Drive, Dropbox, etc.)
            </p>
          </div>
        </div>

        {/* Actions */}
        <div className="flex gap-3 pt-4">
          <button
            type="submit"
            disabled={submitting}
            className="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50"
          >
            {submitting ? "Creating..." : "Create Application"}
          </button>
          <Link
            href="/applications"
            className="px-6 py-2 bg-gray-200 text-gray-800 rounded-lg hover:bg-gray-300 transition-colors"
          >
            Cancel
          </Link>
        </div>
      </form>
    </div>
  );
}
