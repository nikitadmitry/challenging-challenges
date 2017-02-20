import { PageRule } from "../../shared/models/PageRule";
import { ChallengeSearchType } from "./ChallengeSearchType";

export class ChallengesSearchOptions {
    keyword: string;
    pageRule: PageRule;
    searchTypes: ChallengeSearchType[];
}