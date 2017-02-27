import {Component, Input, Output, EventEmitter} from "@angular/core";
import { TranslationService, Translation } from "angular-l10n";

import { ChallengeInfoViewModel } from "../models/ChallengeInfoViewModel";
import {RedirectSearchModel} from "../models/RedirectSearchModel";
import {ChallengeSearchType} from "../models/ChallengeSearchType";
import {EnumPipe} from "../../shared/pipes/enum.pipe";
import {Router} from "@angular/router";

@Component({
    selector: "challenge",
    template: require("./challenge.component.html"),
    styles: [require("./challenge.component.css")],
    providers: [EnumPipe]
})
export class ChallengeComponent extends Translation {
    @Input()
    challenge: ChallengeInfoViewModel;

    @Output("redirect")
    redirectEmitter: EventEmitter<RedirectSearchModel> = new EventEmitter<RedirectSearchModel>();

    constructor(translationService: TranslationService, private enumPipe: EnumPipe,
        private router: Router) {
        super(translationService);
    }

    openChallenge(): void {
        this.router.navigate(["./challenge", this.challenge.id]);
    }

    searchBySection(section: number): void {
        var keyword = this.translation.translate(this.enumPipe.transform(section, "section"));
        this.redirect(ChallengeSearchType.Section, keyword);
    }

    searchByDifficulty(difficulty: number): void {
        var keyword = this.translation.translate(this.enumPipe.transform(difficulty, "difficulty"));
        this.redirect(ChallengeSearchType.Difficulty, keyword);
    }

    searchByLanguage(language: number): void {
        var keyword = this.translation.translate(this.enumPipe.transform(language, "language"));
        this.redirect(ChallengeSearchType.Language, keyword);
    }

    searchByTag(tag: string): void {
        this.redirect(ChallengeSearchType.Tags, tag);
    }

    private redirect(searchType: ChallengeSearchType, keyword: string): void {
        var redirectModel = new RedirectSearchModel();
        redirectModel.searchType = searchType;
        redirectModel.keyword = keyword;

        this.redirectEmitter.emit(redirectModel);
    }
}