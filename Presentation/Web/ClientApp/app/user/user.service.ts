import {Injectable} from "@angular/core";
import {AuthHttp, JwtHelper} from "angular2-jwt";
import {Actions} from "../shared/actions";
import {Observable} from "rxjs";
import {URLSearchParams} from "@angular/http";

@Injectable()
export class UserService {
    private jwtHelper = new JwtHelper();

    constructor(private authHttp: AuthHttp) {

    }

    getCurrentUserId(): Observable<string> {
        return this.authHttp.tokenStream
            .map(token => this.jwtHelper.decodeToken(token)["sub"]);
    }

    getUser(userId: string): Observable<UserModel> {
        let params = new URLSearchParams();
        params.set('userId', userId);

        return this.authHttp.get(Actions.user.getUserById, { search: params })
            .map(response => response.json() as UserModel);
    }

    setAbout(about: string): Observable<any> {
        let params = new URLSearchParams();
        params.set('about', about);

        return this.authHttp.get(Actions.user.setAbout, { search: params });
    }
}