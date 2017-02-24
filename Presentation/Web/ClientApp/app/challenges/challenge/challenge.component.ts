import {Component, Input, Output, EventEmitter} from "@angular/core";
import { TranslationService, Translation } from "angular-l10n";

import { ChallengeInfoViewModel } from "../models/ChallengeInfoViewModel";
import {RedirectSearchModel} from "../models/RedirectSearchModel";
import {ChallengeSearchType} from "../models/ChallengeSearchType";
import {EnumPipe} from "../../shared/pipes/enum.pipe";

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

    constructor(translationService: TranslationService, private enumPipe: EnumPipe) {
        super(translationService);
    }

    searchBySection(section: number): void {
        var keyword = this.translation.translate(this.enumPipe.transform(section, "section"));
        this.redirect(ChallengeSearchType.Section, keyword);
    }

    private redirect(searchType: ChallengeSearchType, keyword: string): void {
        var redirectModel = new RedirectSearchModel();
        redirectModel.searchType = searchType;
        redirectModel.keyword = keyword;

        this.redirectEmitter.emit(redirectModel);
    }
}