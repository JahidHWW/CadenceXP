import { getUserAchievements } from "@/lib/api";
import AchievementCard from "@/components/AchievementCard";

export const dynamic = "force-dynamic";

export default async function AchievementsPage() {
    const achievements = await getUserAchievements(1);

    const earnedAchievements = achievements.filter(
        (achievement) => achievement.isEarned
    );

    const inProgressAchievements = achievements.filter(
        (achievement) => !achievement.isEarned
    );

    return (
        <main className="min-h-screen bg-zinc-950 px-6 py-10 text-white">
            <div className="mx-auto max-w-4xl">
                <div className="mb-8">
                    <h1 className="text-3xl font-bold">
                        Achievements
                    </h1>

                    <p className="mt-2 text-zinc-400">
                        Permanent milestones based on your lifetime riding history.
                    </p>
                </div>

                <section>
                    <div className="mb-4 flex items-center justify-between">
                        <h2 className="text-2xl font-semibold">
                            Earned
                        </h2>

                        <span className="text-sm text-zinc-500">
                            {earnedAchievements.length} earned
                        </span>
                    </div>

                    <div className="space-y-4">
                        {earnedAchievements.map((achievement) => (
                            <AchievementCard
                                key={achievement.achievementId}
                                achievement={achievement}
                            />
                        ))}

                        {earnedAchievements.length === 0 && (
                            <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                                <p className="text-sm text-zinc-500">
                                    No achievements earned yet.
                                </p>
                            </div>
                        )}
                    </div>
                </section>

                <section className="mt-10">
                    <div className="mb-4 flex items-center justify-between">
                        <h2 className="text-2xl font-semibold">
                            In Progress
                        </h2>

                        <span className="text-sm text-zinc-500">
                            {inProgressAchievements.length} remaining
                        </span>
                    </div>

                    <div className="space-y-4">
                        {inProgressAchievements.map((achievement) => (
                            <AchievementCard
                                key={achievement.achievementId}
                                achievement={achievement}
                            />
                        ))}

                        {inProgressAchievements.length === 0 && (
                            <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-5">
                                <p className="text-sm text-zinc-500">
                                    All achievements earned.
                                </p>
                            </div>
                        )}
                    </div>
                </section>
            </div>
        </main>
    );
}