"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { uploadRide } from "@/lib/api";

export default function UploadRidePage() {
    const [name, setName] = useState("");
    const [file, setFile] = useState<File | null>(null);
    const router = useRouter();
    const [isUploading, setIsUploading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(
        event: React.FormEvent<HTMLFormElement>
    ) {
        event.preventDefault();

        if (name.trim().length < 2) {
            setError("Ride name must be at least 2 characters.");
            return;
        }

        if (!file) {
            setError("Please select a GPX file.");
            return;
        }

        if (!file.name.toLowerCase().endsWith(".gpx")) {
            setError("Please select a valid GPX file.");
            return;
        }

        if (!file) {
            setError("Please select a GPX file.");
            return;
        }

        setIsUploading(true);
        setError(null);

        try {
            const formData = new FormData();

            formData.append("Name", name);
            formData.append("UserId", "1");
            formData.append("BikeId", "1");
            formData.append("File", file);

            const ride = await uploadRide(formData);

            router.push(`/rides/${ride.id}`);
        } catch (error) {
            if (error instanceof Error) {
                setError(error.message)
            } else {
                setError("Failed to upload ride.");
            }
        } finally {
            setIsUploading(false);
        }
    }

    return (
        <main className="min-h-screen bg-zinc-950 text-white">
            <div className="mx-auto max-w-2xl px-6 py-16">
                <p className="text-sm font-semibold uppercase tracking-widest text-lime-400">
                    CadenceXP
                </p>

                <h1 className="mt-4 text-4xl font-bold">
                    Upload Ride
                </h1>
                <form
                    onSubmit={handleSubmit}
                    className="mt-10 space-y-6"
                >
                    <div>
                        <label className="mb-2 block text-sm text-zinc-400">
                            Ride Name
                        </label>

                        <input
                            type="text"
                            value={name}
                            onChange={(event) =>
                                setName(event.target.value)
                            }
                            className="w-full rounded-lg border border-zinc-700 bg-zinc-900 px-4 py-3"
                        />
                    </div>

                    <div>
                        <label className="mb-2 block text-sm text-zinc-400">
                            GPX File
                        </label>

                        <label
                            htmlFor="gpx-file"
                            className="inline-block cursor-pointer rounded-lg border border-zinc-700 bg-zinc-800 px-5 py-3 font-semibold text-white transition hover:bg-zinc-700"
                        >
                            Choose GPX File
                        </label>

                        <input
                            id="gpx-file"
                            type="file"
                            accept=".gpx"
                            onChange={(event) =>
                                setFile(event.target.files?.[0] ?? null)
                            }
                            className="hidden"
                        />

                        <p className="mt-3 text-sm text-zinc-400">
                            {file ? file.name : "No file selected"}
                        </p>
                    </div>
                    {error && (
                        <p className="text-sm text-red-400">
                            {error}
                        </p>
                    )}

                    <button
                        type="submit"
                        disabled={isUploading}
                        className="w-full rounded-lg bg-lime-400 px-4 py-3 font-semibold text-black disabled:opacity-50"
                    >
                        {isUploading ? "Uploading..." : "Upload Ride"}
                    </button>
                </form>
            </div>
        </main>
    );
}