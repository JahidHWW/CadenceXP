import Link from "next/link";

import { getRides, getUserProgress } from "@/lib/api";

import {
  metersToMiles,
  metersToFeet,
  formatDuration
} from "@/lib/formatters";

export const dynamic = "force-dynamic";

export default async function Home() {
  const [rides, progress] = await Promise.all([
    getRides(), getUserProgress(1)]);

  const progressPercent =
    progress.xpForNextLevel > 0
      ? Math.min(
        100,
        Math.max(
          0,
          (progress.xpIntoLevel /
            progress.xpForNextLevel) *
          100
        )
      )
      : 100;

  const completedRides = rides.filter(
    (ride) => ride.processingStatus === "Completed"
  );

  const totalDistanceMiles = completedRides.reduce(
    (total, ride) =>
      total + metersToMiles(ride.distanceMeters),
    0
  );

  const totalElevationFeet = completedRides.reduce(
    (total, ride) =>
      total + metersToFeet(ride.elevationGainMeters),
    0
  );

  return (
    <main className="min-h-screen bg-zinc-950 text-white">
      <div className="mx-auto max-w-5xl px-6 py-16">
        <p className="text-sm font-semibold uppercase tracking-widest text-lime-400">
          CadenceXP
        </p>

        <h1 className="mt-4 text-4xl font-bold">
          Ride Dashboard
        </h1>

        <div className="mt-6 flex gap-3">
          <Link
            href="/upload"
            className="rounded-lg bg-lime-400 px-5 py-3 font-semibold text-black transition hover:bg-lime-300"
          >
            Upload Ride
          </Link>

          <Link
            href="/bikes"
            className="rounded-lg border border-zinc-700 px-5 py-3 font-semibold text-white transition hover:bg-zinc-800"
          >
            My Bikes
          </Link>
        </div>

        <div className="mt-10 grid gap-4 sm:grid-cols-2 lg:grid-cols-5">
          <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
            <p className="text-sm text-zinc-500">
              Level
            </p>

            <p className="mt-2 text-2xl font-bold text-lime-400">
              {progress.level}
            </p>
          </div>

          <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
            <p className="text-sm text-zinc-500">
              Total XP
            </p>

            <p className="mt-2 text-2xl font-bold">
              {progress.totalXp}
            </p>
          </div>

          <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
            <p className="text-sm text-zinc-500">
              Completed Rides
            </p>

            <p className="mt-2 text-2xl font-bold">
              {completedRides.length}
            </p>
          </div>

          <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
            <p className="text-sm text-zinc-500">
              Total Distance
            </p>

            <p className="mt-2 text-2xl font-bold">
              {totalDistanceMiles.toFixed(1)} mi
            </p>
          </div>

          <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
            <p className="text-sm text-zinc-500">
              Total Elevation
            </p>

            <p className="mt-2 text-2xl font-bold">
              {Math.round(totalElevationFeet)} ft
            </p>
          </div>
        </div>

        <div className="mt-6 rounded-xl border border-zinc-800 bg-zinc-900 p-5">
          <div className="flex items-center justify-between">
            <p className="font-semibold">
              Level {progress.level}
            </p>

            <p className="text-sm text-zinc-400">
              {progress.xpIntoLevel} / {progress.xpForNextLevel} XP
            </p>
          </div>

          <div className="mt-3 h-3 w-full overflow-hidden rounded-full bg-zinc-800">
            <div
              className="h-full rounded-full bg-lime-400"
              style={{
                width: `${progressPercent}%`
              }}
            />
          </div>
        </div>

        <div className="mt-10 space-y-4">
          {rides.map((ride) => (
            <Link
              key={ride.id}
              href={`/rides/${ride.id}`}
              className="block"
            >
              <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                <h2 className="text-xl font-semibold">
                  {ride.name}
                </h2>

                <p className="mt-2 text-zinc-400">
                  Bike: {ride.bikeName}
                </p>

                <div className="mt-4 grid grid-cols-4 gap-4">
                  <div>
                    <p className="text-sm text-zinc-500">
                      Distance
                    </p>

                    <p className="font-semibold">
                      {metersToMiles(
                        ride.distanceMeters
                      ).toFixed(1)} mi
                    </p>
                  </div>

                  <div>
                    <p className="text-sm text-zinc-500">
                      Elevation
                    </p>

                    <p className="font-semibold">
                      {Math.round(
                        metersToFeet(
                          ride.elevationGainMeters
                        )
                      )} ft
                    </p>
                  </div>

                  <div>
                    <p className="text-sm text-zinc-500">
                      Duration
                    </p>

                    <p className="font-semibold">
                      {formatDuration(
                        ride.durationSeconds
                      )}
                    </p>
                  </div>

                  <div>
                    <p className="text-sm text-zinc-500">
                      XP
                    </p>

                    <p className="font-semibold text-lime-400">
                      +{ride.xpEarned}
                    </p>
                  </div>
                </div>

                <span className="mt-4 inline-block rounded-full border border-zinc-700 px-3 py-1 text-sm text-zinc-300">
                  {ride.processingStatus}
                </span>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </main>
  );
}