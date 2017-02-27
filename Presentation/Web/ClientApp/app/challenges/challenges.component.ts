import { Component, OnInit } from "@angular/core";
import { Translation, TranslationService } from "angular-l10n";
import "rxjs/add/operator/publishReplay";
import "rxjs/add/operator/merge";
import "rxjs/add/observable/forkJoin";

import { ChallengesService } from "../challenges/challenges.service";
import { ChallengeSearchType } from "./models/ChallengeSearchType";
import { ChallengesSearchOptions } from "./models/ChallengesSearchOptions";
import {PageRule} from "../shared/models/PageRule";
import {RedirectSearchModel} from "./models/RedirectSearchModel";

@Component({
    selector: "challenges",
    template: require("./challenges.component.html"),
    styles: [require("./challenges.component.css")],
    providers: [ChallengesService]
})
export class ChallengesComponent extends Translation implements OnInit {
    private PAGE_SIZE: number = 5;
    selectedSearchType: ChallengeSearchType;
    searchString: string;
    challenges: any[];
    isLoading: boolean = true;
    private currentPage: number = 0;
    previousPageEnabled: boolean = false;
    nextPageEnabled: boolean = false;
    noChallenges: boolean = false;

    constructor(private challengesService: ChallengesService, translationService: TranslationService) {
        super(translationService);

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();
    }

    ngOnInit(): void {
        this.searchChallenges();
    }

    searchChallenges(): void {
        this.isLoading = true;
        this.previousPageEnabled = this.currentPage > 0;

        this.challengesService.search(this.getSearchOptions()).subscribe(challenges => {
            setTimeout(() => {
                this.challenges = challenges;
                this.isLoading = false;
            }, 100); // this is to fix animation lag on Search Query field
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
}
