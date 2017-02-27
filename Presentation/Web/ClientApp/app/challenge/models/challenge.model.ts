import {ChallengeType} from "./ChallengeType";
export class ChallengeDetailsModel {
    id: string;
    title: string;
    condition: string;
    difficulty: number;
    section: number;
    rating: number;
    challengeType: ChallengeType;
    authorId: string;
    authorName: string;
    isAuthor: boolean;
    isSolved: boolean;
}