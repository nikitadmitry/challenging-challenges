import { OnInit, Input } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { PaginationInstance } from "ng2-pagination";
import "rxjs/add/operator/merge";
import "rxjs/add/operator/last";

import { SortingType } from "./models/SortingType";
import { SortedPageRule } from "./models/SortedPageRule";
import { ChallengeCardViewModel } from "./challenge-card.model";
import { HomeService } from "../home.service";

const PAGE_SIZE: number = 6;

export abstract class ChallengesComponent implements OnInit {
    protected abstract sortingType: SortingType;
    protected abstract componentTitle: string;

    protected abstract paginatorId: string;

    challengesLoaded: boolean = false;
    challenges: Array<ChallengeCardViewModel> = new Array<ChallengeCardViewModel>();
    pageChanging: boolean = false;
    loadingSpinnerActive = () => !this.challengesLoaded || this.pageChanging;

    @Input("challengesCount")
    challengesCountObservable: Observable<number>;

    config: PaginationInstance = {
        id: this.paginatorId,
        itemsPerPage: PAGE_SIZE,
        currentPage: 1
    };

    constructor(private homeService: HomeService) { }

    ngOnInit(): void {
        var challengesObservable: Observable<ChallengeCardViewModel[]> = this.loadChallenges();

        this.challengesCountObservable.subscribe(count => {
            this.config.totalItems = count;
        });

        this.challengesCountObservable.merge(challengesObservable).last().subscribe((x) => {
            this.challengesLoaded = true;
        });
    }

    private loadChallenges(): Observable<ChallengeCardViewModel[]> {
        this.pageChanging = true;
        var challengesObservable: Observable<ChallengeCardViewModel[]>
            = this.homeService.getChallenges(this.getPageRule());

        challengesObservable.subscribe((challenges) => {
            this.challenges = challenges;
            this.pageChanging = false;
        });

        return challengesObservable;
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
