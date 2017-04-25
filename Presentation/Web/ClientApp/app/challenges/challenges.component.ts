import {Component, OnInit, AfterViewInit, OnDestroy} from "@angular/core";
import { Translation, TranslationService } from "angular-l10n";
import "rxjs/add/operator/publishReplay";
import "rxjs/add/operator/merge";
import "rxjs/add/observable/forkJoin";

import { ChallengesService } from "./challenges.service";
import { ChallengeSearchType } from "./models/ChallengeSearchType";
import { ChallengesSearchOptions } from "./models/ChallengesSearchOptions";
import {PageRule} from "../shared/models/PageRule";
import {RedirectSearchModel} from "./models/RedirectSearchModel";
import {ActivatedRoute} from "@angular/router";
import {Subject} from "rxjs";

@Component({
    selector: "challenges",
    template: require("./challenges.component.html"),
    styles: [require("./challenges.component.css")],
    providers: [ChallengesService]
})
export class ChallengesComponent extends Translation implements OnInit, OnDestroy {
    private ngUnsubscribe: Subject<void> = new Subject<void>();
    private PAGE_SIZE: number = 5;
    selectedSearchType: ChallengeSearchType;
    searchString: string;
    challenges: any[];
    isLoading: boolean = true;
    private currentPage: number = 0;
    previousPageEnabled: boolean = false;
    nextPageEnabled: boolean = false;
    noChallenges: boolean = false;

    constructor(private challengesService: ChallengesService, translationService: TranslationService,
                private route: ActivatedRoute) {
        super(translationService);

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();
    }

    ngOnInit(): void {
        this.loadFilters();
        this.parseRouteParameters();
        this.searchChallenges();
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.subscribe();
    }

    searchChallenges(): void {
        this.isLoading = true;
        this.previousPageEnabled = this.currentPage > 0;

        this.challengesService.search(this.getSearchOptions()).takeUntil(this.ngUnsubscribe)
            .subscribe(challenges => {
                this.challenges = challenges;
                this.isLoading = false;
                this.nextPageEnabled = challenges.length === this.PAGE_SIZE;
                this.noChallenges = challenges.length === 0;
            });
    }

    redirect(redirectModel: RedirectSearchModel): void {
        this.selectedSearchType = redirectModel.searchType;
        this.searchString = redirectModel.keyword;
        this.onChangeFilter();
    }

    onChangeFilter(): void {
        this.currentPage = 0;
        this.saveFilters();
        this.searchChallenges();
    }

    private getSearchOptions(): ChallengesSearchOptions {
        let searchOptions: ChallengesSearchOptions = new ChallengesSearchOptions();
        let pageRule = new PageRule();
        pageRule.count = this.PAGE_SIZE;
        pageRule.start = this.currentPage * this.PAGE_SIZE;

        searchOptions.pageRule = pageRule;
        searchOptions.keyword = this.searchString;
        if (this.selectedSearchType) {
            searchOptions.searchTypes = [this.selectedSearchType];
        }

        return searchOptions;
    }

    nextPage(): void {
        this.currentPage++;
        this.searchChallenges();
    }

    previousPage(): void {
        this.currentPage--;
        this.searchChallenges();
    }

    private saveFilters() {
        localStorage.setItem("search_filters",
            JSON.stringify({ searchString: this.searchString, searchType: this.selectedSearchType }));
    }

    private loadFilters() {
        const filtersString = localStorage.getItem("search_filters");
        if (filtersString) {
            let filters = JSON.parse(filtersString);
            this.searchString = filters.searchString;
            this.selectedSearchType = filters.searchType;
        }
    }

    private parseRouteParameters() {
        this.route.params.takeUntil(this.ngUnsubscribe).subscribe(params => {
            let searchText = params['searchText'];
            if (searchText !== undefined) {
                this.selectedSearchType = ChallengeSearchType.Title;
                this.searchString = searchText;
            }
        });
    }
}
