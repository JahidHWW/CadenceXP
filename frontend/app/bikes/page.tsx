import Link from "next/link";
import AddBikeForm from "@/components/AddBikeForm";
import { getUserBikes } from "@/lib/api";

export const dynamic = "force-dynamic";

export default async function BikesPage() {
    const bikes = await getUserBikes(1);

    return (
        <main className="min-h-screen bg-zinc-950 text-white">
            <div className="mx-auto max-w-4xl px-6 py-16">
                <p className="text-sm font-semibold uppercase tracking-widest text-lime-400">
                    CadenceXP
                </p>

                <h1 className="mt-4 text-4xl font-bold">
                    My Bikes
                </h1>

                <p className="mt-2 text-zinc-400">
                    Manage the bikes you use for your rides.
                </p>

                <AddBikeForm />

                <div className="mt-10 space-y-4">
                    {bikes.map((bike) => (
                        <div
                            key={bike.id}
                            className="rounded-xl border border-zinc-800 bg-zinc-900 p-5"
                        >
                            <h2 className="text-xl font-semibold">
                                {bike.name}
                            </h2>

                            <p className="mt-2 text-zinc-400">
                                {bike.type}
                            </p>
                        </div>
                    ))}
                </div>

                <Link
                    href="/"
                    className="mt-8 inline-block text-sm text-lime-400 hover:text-lime-300"
                >
                    ← Back to dashboard
                </Link>
            </div>
        </main>
    );
}