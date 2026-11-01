import type { UserQuest } from "@/lib/api";

type QuestCardProps = {
    quest: UserQuest;
    status: "active" | "locked" | "completed";
};

export default function QuestCard({
    quest,
    status
}: QuestCardProps) {
    const progressPercent =
        quest.targetValue > 0
            ? Math.min(
                100,
                (quest.progress / quest.targetValue) * 100
            )
            : 0;

    let progressLabel;

    switch (quest.type) {
        case "Distance":
        case "SingleRideDistance":
            progressLabel =
                `${quest.progress.toFixed(1)} / ${quest.targetValue.toFixed(1)} mi`;
            break;

        case "Elevation":
            progressLabel =
                `${quest.progress.toFixed(0)} / ${quest.targetValue.toFixed(0)} m`;
            break;

        case "RideCount":
            progressLabel =
                `${quest.progress.toFixed(0)} / ${quest.targetValue.toFixed(0)} rides`;
            break;

        default:
            progressLabel =
                `${quest.progress} / ${quest.targetValue}`;
    }

    function getDifficultyClasses(difficulty: string) {
        switch (difficulty) {
            case "Hard":
                return "border-orange-700 bg-orange-950/40 text-orange-300";

            case "Elite":
                return "border-purple-700 bg-purple-950/40 text-purple-300";

            default:
                return "border-zinc-700 bg-zinc-800 text-zinc-300";
        }
    }

    return (
        <div
            className={`rounded-xl border p-5 ${status === "completed"
                ? "border-lime-900 bg-zinc-900"
                : status === "locked"
                    ? "border-zinc-800 bg-zinc-900/60"
                    : "border-zinc-800 bg-zinc-900"
                }`}
        >
            <div className="flex items-start justify-between gap-4">
                <div>
                    <div className="flex items-center gap-3">
                        <h3 className="text-lg font-semibold">
                            {quest.name}
                        </h3>

                        <span
                            className={`rounded-full border px-2 py-1 text-xs ${getDifficultyClasses(
                                quest.difficulty
                            )}`}
                        >
                            {quest.difficulty}
                        </span>
                    </div>

                    <p className="mt-2 text-sm text-zinc-400">
                        {quest.description}
                    </p>

                    {status === "active" && (
                        <p className="mt-2 text-xs text-zinc-500">
                            Unlocked at Level {quest.requiredLevel}
                        </p>
                    )}
                </div>

                <div className="text-right">
                    <p
                        className={
                            status === "locked"
                                ? "font-semibold text-zinc-500"
                                : "font-semibold text-lime-400"
                        }
                    >
                        +{quest.xpReward} XP
                    </p>

                    <p className="mt-1 text-sm text-zinc-500">
                        {quest.type === "SingleRideDistance"
                            ? "Single Ride Distance"
                            : quest.type}
                    </p>
                </div>
            </div>

            {status === "locked" ? (
                <div className="mt-4 flex items-center justify-between">
                    <p className="text-sm font-medium text-zinc-400">
                        Locked
                    </p>

                    <p className="text-sm text-zinc-500">
                        Unlocks at Level {quest.requiredLevel}
                    </p>
                </div>
            ) : (
                <>
                    <div className="mt-5">
                        <div className="mb-2 flex items-center justify-between">
                            <span className="text-sm text-zinc-300">
                                Progress
                            </span>

                            <span className="text-sm text-zinc-400">
                                {status === "completed"
                                    ? "Complete"
                                    : progressLabel}
                            </span>
                        </div>

                        <div className="h-3 w-full overflow-hidden rounded-full bg-zinc-800">
                            <div
                                className="h-full rounded-full bg-lime-400"
                                style={{
                                    width:
                                        status === "completed"
                                            ? "100%"
                                            : `${progressPercent}%`
                                }}
                            />
                        </div>
                    </div>

                    <p
                        className={`mt-4 ${status === "completed"
                            ? "font-medium text-lime-400"
                            : "text-sm text-zinc-500"
                            }`}
                    >
                        {status === "completed"
                            ? "Completed"
                            : "In Progress"}
                    </p>
                </>
            )}
        </div>
    );
}