"use client";

import { useEffect, useState } from "react";
import dynamic from "next/dynamic";

import type { TrackPoint } from "@/types/trackPoint";

const RideMap = dynamic(
    () => import("@/components/RideMap"),
    {
        ssr: false,
        loading: () => (
            <div className="h-[400px] w-full rounded-xl bg-zinc-900" />
        )
    }
);

type RideMapClientProps = {
    trackPoints: TrackPoint[];
};

export default function RideMapClient({
    trackPoints
}: RideMapClientProps) {
    const [mounted, setMounted] = useState(false);

    useEffect(() => {
        const frame = requestAnimationFrame(() => {
            setMounted(true);
        });

        return () => {
            cancelAnimationFrame(frame);
        };
    }, []);

    if (!mounted) {
        return (
            <div className="h-[400px] w-full rounded-xl bg-zinc-900" />
        );
    }

    return <RideMap trackPoints={trackPoints} />;
}