import { getUserQuests } from "@/lib/api";

export const dynamic = "force-dynamic";

export default async function QuestsPage() {
    const quests = await getUserQuests(1);

    return (
        <main className="min-h-screen bg-zinc-950 px-6 py-10 text-white">
            <div className="mx-auto max-w-4xl">
                <div className="mb-8">
                    <h1 className="text-3xl font-bold">Quests</h1>

                    <p className="mt-2 text-zinc-400">
                        Complete cycling goals to earn XP and level up.
                    </p>
                </div>

                <div className="space-y-4">
                    {quests.map((quest) => {
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

                        return (
                            <div
                                key={quest.questId}
                                className="rounded-xl border border-zinc-800 bg-zinc-900 p-5"
                            >
                                <div className="flex items-start justify-between gap-4">
                                    <div>
                                        <h2 className="text-xl font-semibold">
                                            {quest.name}
                                        </h2>

                                        <p className="mt-1 text-sm text-zinc-400">
                                            {quest.description}
                                        </p>
                                    </div>

                                    <div className="text-right">
                                        <p className="font-semibold text-lime-400">
                                            +{quest.xpReward} XP
                                        </p>

                                        <p className="mt-1 text-sm text-zinc-400">
                                            {quest.type}
                                        </p>
                                    </div>
                                </div>

                                <div className="mt-5">
                                    <div className="mb-2 flex items-center justify-between">
                                        <span className="text-sm text-zinc-300">
                                            Progress
                                        </span>

                                        <span className="text-sm text-zinc-400">
                                            {progressLabel}
                                        </span>
                                    </div>

                                    <div className="h-3 w-full overflow-hidden rounded-full bg-zinc-800">
                                        <div
                                            className="h-full rounded-full bg-lime-400"
                                            style={{
                                                width: `${progressPercent}%`
                                            }}
                                        />
                                    </div>
                                </div>

                                <div className="mt-4">
                                    {quest.isCompleted ? (
                                        <p className="font-medium text-lime-400">
                                            Completed
                                        </p>
                                    ) : (
                                        <p className="text-sm text-zinc-500">
                                            In Progress
                                        </p>
                                    )}
                                </div>
                            </div>
                        );
                    })}
                </div>
            </div>
        </main>
    );
}