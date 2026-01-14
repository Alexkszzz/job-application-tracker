"use client";

import { useEffect, useState } from "react";
import { applicationService } from "@/lib/api/applications";
import { Application, ApplicationStatus } from "@/lib/api/types";
import Link from "next/link";

export default function Dashboard() {
  const [applications, setApplications] = useState<Application[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchApplications = async () => {
      try {
        const data = await applicationService.getAll();
        setApplications(data);
      } catch (err) {
        setError("Failed to load applications");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchApplications();
  }, []);

  if (loading) {
    return <div className="text-center py-12">Loading...</div>;
  }

  if (error) {
    return <div className="text-center py-12 text-red-600">{error}</div>;
  }

  const stats = {
    total: applications.length,
    applied: applications.filter((a) => a.status === ApplicationStatus.Applied)
      .length,
    interview: applications.filter(
      (a) => a.status === ApplicationStatus.Interview
    ).length,
    offer: applications.filter((a) => a.status === ApplicationStatus.Offer)
      .length,
    rejected: applications.filter(
      (a) => a.status === ApplicationStatus.Rejected
    ).length,
  };

  return (
    <div>
      <h1 className="text-3xl font-bold text-gray-900 mb-8">Dashboard</h1>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-5 gap-6 mb-8">
        <div className="bg-white p-6 rounded-lg shadow">
          <div className="text-sm text-gray-600">Total Applications</div>
          <div className="text-3xl font-bold text-gray-900 mt-2">
            {stats.total}
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="text-sm text-gray-600">Applied</div>
          <div className="text-3xl font-bold text-blue-600 mt-2">
            {stats.applied}
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="text-sm text-gray-600">Interview</div>
          <div className="text-3xl font-bold text-yellow-600 mt-2">
            {stats.interview}
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="text-sm text-gray-600">Offer</div>
          <div className="text-3xl font-bold text-green-600 mt-2">
            {stats.offer}
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="text-sm text-gray-600">Rejected</div>
          <div className="text-3xl font-bold text-red-600 mt-2">
            {stats.rejected}
          </div>
        </div>
      </div>

      {/* Recent Applications */}
      <div className="bg-white rounded-lg shadow">
        <div className="px-6 py-4 border-b border-gray-200">
          <h2 className="text-xl font-semibold text-gray-900">
            Recent Applications
          </h2>
        </div>
        <div className="p-6">
          {applications.length === 0 ? (
            <div className="text-center py-12">
              <p className="text-gray-500 mb-4">No applications yet</p>
              <Link
                href="/applications/new"
                className="inline-block bg-blue-600 text-white px-6 py-2 rounded-md hover:bg-blue-700"
              >
                Add Your First Application
              </Link>
            </div>
          ) : (
            <div className="space-y-4">
              {applications.slice(0, 5).map((app) => (
                <Link
                  key={app.id}
                  href={`/applications/${app.id}`}
                  className="block p-4 border border-gray-200 rounded-lg hover:bg-gray-50"
                >
                  <div className="flex justify-between items-start">
                    <div>
                      <h3 className="font-semibold text-gray-900">
                        {app.jobTitle}
                      </h3>
                      <p className="text-gray-600">{app.companyName}</p>
                    </div>
                    <StatusBadge status={app.status} />
                  </div>
                </Link>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

function StatusBadge({ status }: { status: ApplicationStatus }) {
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
}
