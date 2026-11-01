import { getUserProgress, getUserQuests } from "@/lib/api";
import QuestCard from "@/components/QuestCard";

export const dynamic = "force-dynamic";

export default async function QuestsPage() {
    const [quests, progress] = await Promise.all([
        getUserQuests(1),
        getUserProgress(1)
    ]);

    const activeQuests = quests.filter(
        (quest) => quest.isUnlocked && !quest.isCompleted
    );

    const lockedQuests = quests.filter(
        (quest) => !quest.isUnlocked
    );

    const completedQuests = quests.filter(
        (quest) => quest.isCompleted
    );

    const visibleLockedQuests = [...lockedQuests]
        .sort((a, b) => a.requiredLevel - b.requiredLevel)
        .slice(0, 8);

    const questXpEarned = completedQuests.reduce(
        (total, quest) => total + quest.xpReward,
        0
    );

    const levelProgressPercent =
        progress.xpForNextLevel > 0
            ? Math.min(
                100,
                (progress.xpIntoLevel / progress.xpForNextLevel) * 100
            )
            : 100;

    return (
        <main className="min-h-screen bg-zinc-950 px-6 py-10 text-white">
            <div className="mx-auto max-w-4xl">
                <div className="mb-8">
                    <h1 className="text-3xl font-bold">
                        Quests
                    </h1>

                    <p className="mt-2 text-zinc-400">
                        Complete cycling goals to earn XP and level up.
                    </p>

                    <div className="mt-6 grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
                        <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-4">
                            <p className="text-sm text-zinc-400">
                                Current Level
                            </p>

                            <p className="mt-1 text-2xl font-bold text-lime-400">
                                {progress.level}
                            </p>
                        </div>

                        <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-4">
                            <p className="text-sm text-zinc-400">
                                Active Quests
                            </p>

                            <p className="mt-1 text-2xl font-bold">
                                {activeQuests.length}
                            </p>
                        </div>

                        <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-4">
                            <p className="text-sm text-zinc-400">
                                Completed Quests
                            </p>

                            <p className="mt-1 text-2xl font-bold">
                                {completedQuests.length}
                            </p>
                        </div>

                        <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-4">
                            <p className="text-sm text-zinc-400">
                                Quest XP Earned
                            </p>

                            <p className="mt-1 text-2xl font-bold text-lime-400">
                                {questXpEarned}
                            </p>
                        </div>
                    </div>

                    <div className="mt-4 rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                        <div className="mb-2 flex items-center justify-between">
                            <div>
                                <p className="font-semibold">
                                    Level {progress.level} Progress
                                </p>

                                <p className="mt-1 text-sm text-zinc-500">
                                    {progress.totalXp} total XP
                                </p>
                            </div>

                            {progress.xpForNextLevel > 0 ? (
                                <p className="text-sm text-zinc-400">
                                    {progress.xpIntoLevel} /{" "}
                                    {progress.xpForNextLevel} XP
                                </p>
                            ) : (
                                <p className="font-medium text-lime-400">
                                    Max Level
                                </p>
                            )}
                        </div>

                        <div className="h-3 w-full overflow-hidden rounded-full bg-zinc-800">
                            <div
                                className="h-full rounded-full bg-lime-400"
                                style={{
                                    width: `${levelProgressPercent}%`
                                }}
                            />
                        </div>

                        {progress.level < 50 && (
                            <p className="mt-2 text-sm text-zinc-500">
                                {progress.xpForNextLevel -
                                    progress.xpIntoLevel}{" "}
                                XP until Level {progress.level + 1}
                            </p>
                        )}
                    </div>
                </div>

                <section>
                    <div className="mb-4 flex items-center justify-between">
                        <h2 className="text-2xl font-semibold">
                            Active Quests
                        </h2>

                        <span className="text-sm text-zinc-500">
                            {activeQuests.length} active
                        </span>
                    </div>

                    <div className="space-y-4">
                        {activeQuests.map((quest) => (
                            <QuestCard
                                key={quest.questId}
                                quest={quest}
                                status="active"
                            />
                        ))}

                        {activeQuests.length === 0 && (
                            <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                                <p className="text-sm text-zinc-500">
                                    No active quests right now.
                                </p>
                            </div>
                        )}
                    </div>
                </section>

                <section className="mt-10">
                    <div className="mb-4 flex items-center justify-between">
                        <h2 className="text-2xl font-semibold">
                            Locked Quests
                        </h2>

                        <span className="text-sm text-zinc-500">
                            {lockedQuests.length} locked
                        </span>
                    </div>

                    <div className="space-y-4">
                        {visibleLockedQuests.map((quest) => (
                            <QuestCard
                                key={quest.questId}
                                quest={quest}
                                status="locked"
                            />
                        ))}

                        {lockedQuests.length >
                            visibleLockedQuests.length && (
                                <p className="pt-3 text-center text-sm text-zinc-500">
                                    +
                                    {lockedQuests.length -
                                        visibleLockedQuests.length}{" "}
                                    more quests unlock at higher levels
                                </p>
                            )}

                        {lockedQuests.length === 0 && (
                            <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                                <p className="text-sm text-zinc-500">
                                    All quests are unlocked.
                                </p>
                            </div>
                        )}
                    </div>
                </section>

                <section className="mt-10">
                    <div className="mb-4 flex items-center justify-between">
                        <h2 className="text-2xl font-semibold">
                            Completed Quests
                        </h2>

                        <span className="text-sm text-zinc-500">
                            {completedQuests.length} completed
                        </span>
                    </div>

                    <div className="space-y-4">
                        {completedQuests.map((quest) => (
                            <QuestCard
                                key={quest.questId}
                                quest={quest}
                                status="completed"
                            />
                        ))}

                        {completedQuests.length === 0 && (
                            <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                                <p className="text-sm text-zinc-500">
                                    You haven&apos;t completed any quests yet.
                                </p>
                            </div>
                        )}
                    </div>
                </section>
            </div>
        </main>
    );
}