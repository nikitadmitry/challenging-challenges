import { OnInit } from "@angular/core";

import { SortingType } from "../../shared/models/SortingType";
import { SortedPageRule } from "../../shared/models/SortedPageRule";
import { ChallengeCardViewModel } from "./challenge-card.model";
import { HomeService } from "../home.service";

const PAGE_SIZE: number = 9;

export abstract class ChallengesComponent implements OnInit {
    protected abstract sortingType: SortingType;
    protected abstract componentTitle: string;

    challenges: Array<ChallengeCardViewModel>;
    page: number = 0;

    constructor(private homeService: HomeService) { }

    ngOnInit(): void {
        this.homeService.getChallenges(this.getPageRule())
            .subscribe((challenges) => this.challenges = challenges);
    }

    private getPageRule(): SortedPageRule {
        let pageRule: SortedPageRule = new SortedPageRule();
        pageRule.count = PAGE_SIZE;
        pageRule.start = this.page * PAGE_SIZE;
        pageRule.sortingType = this.sortingType;

        return pageRule;
    }
}
