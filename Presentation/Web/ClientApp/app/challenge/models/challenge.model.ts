import {ChallengeType} from "./ChallengeType";
import {Difficulty} from "./Difficulty";
import {Section} from "./Section";

export class ChallengeDetailsModel {
    id: string;
    title: string;
    condition: string;
    difficulty: Difficulty;
    section: Section;
    rating: number;
    challengeType: ChallengeType;
    authorId: string;
    authorName: string;
    isAuthor: boolean;
    isSolved: boolean;
    answerTemplate: string;
}