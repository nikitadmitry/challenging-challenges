import { PageRule } from "../../shared/models/PageRule";
import { ChallengeSearchType } from "./ChallengeSearchType";

export class ChallengesPageRule extends PageRule {
    keyword: string;
    searchTypes: ChallengeSearchType[];
}