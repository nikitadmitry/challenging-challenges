import { Component, OnInit, ViewChild } from "@angular/core";
import { MdlSelectComponent } from "@angular2-mdl-ext/select";
import { Translation, TranslationService } from "angular-l10n";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/publishReplay";
import "rxjs/add/operator/merge";
import "rxjs/add/observable/forkJoin";

import { ChallengesService } from "../challenges/challenges.service";
import { ChallengeSearchType } from "./models/ChallengeSearchType";
import { ChallengesPageRule } from "./models/ChallengesPageRule";

@Component({
    selector: "challenges",
    template: require("./challenges.component.html"),
    providers: [ChallengesService]
})
export class ChallengesComponent extends Translation implements OnInit {
    private PAGE_SIZE: number = 10;
    selectedSearchType: ChallengeSearchType = ChallengeSearchType.Title;
    searchString: string;
    challenges: Observable<any>;
    isLoading: boolean = true;
    private currentPage: number = 0;
    previousPageEnabled: boolean = false;
    nextPageEnabled: boolean = false;

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
        this.challenges = this.challengesService.search(this.getPageRule())
            .publishReplay(1)
            .refCount();

        this.challenges.subscribe(challenges => {
            this.previousPageEnabled = this.currentPage > 0;
            this.nextPageEnabled = challenges.length === this.PAGE_SIZE;
            this.isLoading = false;
        });
    }

    changeFilter(): void {
        this.currentPage = 0;
        this.searchChallenges();
    }

    private getPageRule(): ChallengesPageRule {
        let pageRule: ChallengesPageRule = new ChallengesPageRule();
        pageRule.count = this.PAGE_SIZE;
        pageRule.start = this.currentPage * this.PAGE_SIZE;
        pageRule.keyword = this.searchString;
        pageRule.searchTypes = [this.selectedSearchType];

        return pageRule;
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
