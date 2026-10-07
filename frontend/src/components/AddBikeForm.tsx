"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";

import { createBike } from "@/lib/api";

export default function AddBikeForm() {
    const router = useRouter();

    const [name, setName] = useState("");
    const [type, setType] = useState("");
    const [isSaving, setIsSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(
        event: React.FormEvent<HTMLFormElement>
    ) {
        event.preventDefault();

        if (name.trim().length < 2) {
            setError("Bike name must be at least 2 characters.");
            return;
        }

        if (type.trim().length < 2) {
            setError("Bike type must be at least 2 characters.");
            return;
        }

        setIsSaving(true);
        setError(null);

        try {
            await createBike({
                name: name.trim(),
                type: type.trim(),
                userId: 1
            });

            setName("");
            setType("");

            router.refresh();
        } catch (error) {
            if (error instanceof Error) {
                setError(error.message);
            } else {
                setError("Failed to create bike.");
            }
        } finally {
            setIsSaving(false);
        }
    }

    return (
        <form
            onSubmit={handleSubmit}
            className="mt-10 space-y-4 rounded-xl border border-zinc-800 bg-zinc-900 p-6"
        >
            <h2 className="text-xl font-semibold">
                Add Bike
            </h2>

            <div>
                <label className="mb-2 block text-sm text-zinc-400">
                    Bike Name
                </label>

                <input
                    value={name}
                    onChange={(event) =>
                        setName(event.target.value)
                    }
                    className="w-full rounded-lg border border-zinc-700 bg-zinc-950 px-4 py-3"
                />
            </div>

            <div>
                <label className="mb-2 block text-sm text-zinc-400">
                    Bike Type
                </label>

                <input
                    value={type}
                    onChange={(event) =>
                        setType(event.target.value)
                    }
                    className="w-full rounded-lg border border-zinc-700 bg-zinc-950 px-4 py-3"
                />
            </div>

            {error && (
                <p className="text-sm text-red-400">
                    {error}
                </p>
            )}

            <button
                type="submit"
                disabled={isSaving}
                className="rounded-lg bg-lime-400 px-5 py-3 font-semibold text-black disabled:opacity-50"
            >
                {isSaving ? "Adding..." : "Add Bike"}
            </button>
        </form>
    );
}