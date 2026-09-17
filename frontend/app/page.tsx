"use client";

import { useEffect, useState } from "react";

type HealthResponse = {
  status: string;
  application: string;
};

export default function Home() {
  const [health, setHealth] = useState<HealthResponse | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function checkBackend() {
      try {
        const response = await fetch(
          `${process.env.NEXT_PUBLIC_API_URL}/api/health`
        );

        if (!response.ok) {
          throw new Error("Backend request failed");
        }

        const data: HealthResponse = await response.json();

        setHealth(data);
      } catch {
        setError("Unable to connect to the CadenceXP backend.");
      }
    }

    checkBackend();
  }, []);

  return (
    <main className="min-h-screen bg-zinc-950 text-white">
      <div className="mx-auto max-w-5xl px-6 py-20">
        <p className="mb-4 text-sm font-semibold uppercase tracking-widest text-lime-400">
          CadenceXP
        </p>

        <h1 className="max-w-3xl text-5xl font-bold tracking-tight">
          Turn every ride into progress.
        </h1>

        <p className="mt-6 max-w-2xl text-lg text-zinc-400">
          Track your cycling, complete quests, earn XP, unlock achievements,
          and watch your progress grow with every ride.
        </p>

        <div className="mt-10 rounded-xl border border-zinc-800 bg-zinc-900 p-6">
          <h2 className="text-xl font-semibold">System Status</h2>

          {!health && !error && (
            <p className="mt-2 text-zinc-400">Checking backend...</p>
          )}

          {health && (
            <p className="mt-2 text-lime-400">
              {health.application} backend is {health.status}.
            </p>
          )}

          {error && (
            <p className="mt-2 text-red-400">
              {error}
            </p>
          )}
        </div>
      </div>
    </main>
  );
}