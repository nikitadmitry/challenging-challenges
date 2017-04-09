interface UserModel {
    id: string;
    userName: string;
    about: string;
    email: string;
    level: number;
    solvedChallenges: number;
    postedChallenges: number;
    achievements: Array<number>;
    isReadOnly: boolean;
}