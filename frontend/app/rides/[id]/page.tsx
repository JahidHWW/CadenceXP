import {
    getRideById,
    getRideTrackPoints
} from "@/lib/api";
import type { TrackPoint } from "@/types/trackPoint";
//import RideMap from "@/components/RideMapClient";

import {
    metersToMiles,
    metersToFeet,
    formatDuration
} from "@/lib/formatters";
import RideMapClient from "@/components/RideMapClient";

type RideDetailsPageProps = {
    params: Promise<{
        id: string;
    }>;
};

export default async function RideDetailsPage({
    params
}: RideDetailsPageProps) {
    const { id } = await params;

    const ride = await getRideById(Number(id));
    let trackPoints: TrackPoint[] = [];
    try {
        trackPoints = await getRideTrackPoints(Number(id));
    } catch {
        trackPoints = [];
    }

    return (
        <main className="min-h-screen bg-zinc-950 text-white">
            <div className="mx-auto max-w-4xl px-6 py-16">
                <p className="text-sm font-semibold uppercase tracking-widest text-lime-400">
                    CadenceXP
                </p>

                <h1 className="mt-4 text-4xl font-bold">
                    {ride.name}
                </h1>

                <p className="mt-3 text-zinc-400">
                    Bike: {ride.bikeName}
                </p>

                <div className="mt-10 grid gap-4 sm:grid-cols-4">
                    <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                        <p className="text-sm text-zinc-500">
                            Distance
                        </p>

                        <p className="mt-2 text-2xl font-bold">
                            {metersToMiles(ride.distanceMeters).toFixed(1)} mi
                        </p>
                    </div>

                    <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                        <p className="text-sm text-zinc-500">
                            Elevation
                        </p>

                        <p className="mt-2 text-2xl font-bold">
                            {Math.round(
                                metersToFeet(ride.elevationGainMeters)
                            )} ft
                        </p>
                    </div>

                    <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                        <p className="text-sm text-zinc-500">
                            Duration
                        </p>

                        <p className="mt-2 text-2xl font-bold">
                            {formatDuration(ride.durationSeconds)}
                        </p>
                    </div>

                    <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                        <p className="text-sm text-zinc-500">
                            XP Earned
                        </p>

                        <p className="mt-2 text-2xl font-bold text-lime-400">
                            +{ride.xpEarned} XP
                        </p>
                    </div>
                </div>

                <div className="mt-8 rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                    <p className="text-sm text-zinc-500">
                        Ride Date
                    </p>

                    <p className="mt-2 font-semibold">
                        {new Date(ride.rideDateUtc).toLocaleString()}
                    </p>
                </div>

                <span className="mt-6 inline-block rounded-full border border-zinc-700 px-3 py-1 text-sm text-zinc-300">
                    {ride.processingStatus}
                </span>

                <div className="mt-8">
                    <h2 className="mb-4 text-2xl font-bold">
                        Route
                    </h2>

                    <RideMapClient trackPoints={trackPoints} />
                </div>
            </div>
        </main>
    );
}