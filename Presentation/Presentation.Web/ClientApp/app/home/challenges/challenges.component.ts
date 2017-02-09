import { OnInit, Input, ViewChild } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { PaginationInstance } from "ng2-pagination";
import "rxjs/add/operator/merge";
import "rxjs/add/operator/last";

import { SortingType } from "../../shared/models/SortingType";
import { SortedPageRule } from "../../shared/models/SortedPageRule";
import { ChallengeCardViewModel } from "./challenge-card.model";
import { HomeService } from "../home.service";
import { PaginatorComponent } from "../../shared/shared-components/paginator.component";

const PAGE_SIZE: number = 9;

export abstract class ChallengesComponent implements OnInit {
    protected abstract sortingType: SortingType;
    protected abstract componentTitle: string;

    challengesLoaded: boolean = false;
    challenges: Array<ChallengeCardViewModel>;
    pageChanging: boolean = false;
    loadingSpinnerActive = () => !this.challengesLoaded || this.pageChanging;

    @Input("challengesCount")
    challengesCountObservable: Observable<number>;

    @ViewChild(PaginatorComponent)
    paginatorComponent: PaginatorComponent;

    config: PaginationInstance = {
        id: "mdl-paginator",
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
        //this.config.currentPage = newPage;
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
