import { Component, Input } from "@angular/core";

import { ChallengeCardViewModel } from "./challenge-card.model";

@Component({
    selector: "challenge-card",
    template: require("./challenge-card.component.html")
})
export class ChallengeCardComponent {
    @Input()
    challenge: ChallengeCardViewModel;

    showAditionalInfo(): void {
        alert("OPPA");
    }
}