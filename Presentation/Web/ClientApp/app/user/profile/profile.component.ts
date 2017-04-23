import { Component, OnInit, ViewChild } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "../user.service";
import * as $ from "jquery";
import { MdlDialogComponent } from "angular2-mdl/components";
require("jquery-knob");

@Component({
    selector: 'profile',
    template: require("./profile.component.html"),
    styles: [require("./profile.component.css")],
    providers: [UserService]
})
export class ProfileComponent {
    @ViewChild("editAboutDialog") editAboutDialog: MdlDialogComponent;
    model: UserModel;
    newAbout: string;
    isCurrentUser: boolean;

    constructor(route: ActivatedRoute, private userService: UserService) {
        route.params.subscribe((params) => {
            let userId = params["id"];
            this.userService.getUser(userId).subscribe((model) => {
                this.model = model;
                setTimeout(() => this.initKnobs(), 0);
            });
            this.userService.getCurrentUserId()
                .subscribe((currentId) => this.isCurrentUser = userId === currentId);
        });
    }

    saveAbout() {
        this.userService.setAbout(this.newAbout)
            .subscribe(() => {
                this.model.about = this.newAbout;
                this.editAboutDialog.close();
            });
    }

    initKnobs() {
        (<any>$("#user-level")).knob({
            'angleOffset': -125,
            'angleArc': 250,
            'readOnly': true
        });

        (<any>$("#user-solvedChallenges, #user-postedChallenges")).knob({
            'fgColor': '#66CC66',
            'thickness': '.2',
            'angleOffset': 180,
            'readOnly': true
        });
    }

}