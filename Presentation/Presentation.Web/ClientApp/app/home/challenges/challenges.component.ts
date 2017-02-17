import { OnInit, Input } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { PaginationInstance } from "ng2-pagination";
import { TranslationService, Translation } from "angular-l10n";
import "rxjs/add/operator/publishReplay";

import { SortingType } from "./models/SortingType";
import { SortedPageRule } from "./models/SortedPageRule";
import { ChallengeCardViewModel } from "./challenge-card.model";
import { HomeService } from "../home.service";

const PAGE_SIZE: number = 6;

export abstract class ChallengesComponent extends Translation implements OnInit {
    protected abstract sortingType: SortingType;
    protected abstract componentTitleId: string;

    protected abstract paginatorId: string;

    challenges: Observable<ChallengeCardViewModel[]>;
    loadingSpinnerActive = () => this.challengesLoading || this.challengesCountLoading;
    challengesLoading: boolean;
    challengesCountLoading: boolean = true;

    @Input("challengesCount")
    challengesCountObservable: Observable<number>;

    config: PaginationInstance = {
        itemsPerPage: PAGE_SIZE,
        currentPage: 1
    };

    constructor(private homeService: HomeService, translationService: TranslationService) {
        super(translationService);
    }

    ngOnInit(): void {
        this.loadChallenges();

        this.config.id = this.paginatorId;

        this.challengesCountObservable.subscribe(count => {
            this.config.totalItems = count;
            this.challengesCountLoading = false;
        });
    }

    private loadChallenges(): void {
        this.challengesLoading = true;
        this.challenges = this.homeService.getChallenges(this.getPageRule())
            .publishReplay(1)
            .refCount();

        this.challenges.subscribe(() => {
            this.challengesLoading = false;
        });
    }

    changePage(newPage: number): void {
        this.config.currentPage = newPage;
        this.loadChallenges();
    }

    private getPageRule(): SortedPageRule {
        let pageRule: SortedPageRule = new SortedPageRule();
        pageRule.count = PAGE_SIZE;
        pageRule.start = (this.config.currentPage - 1) * PAGE_SIZE;
        pageRule.sortingType = this.sortingType;

        return pageRule;
    }
}
