import { observable, action } from 'mobx';
import * as _ from "lodash";
import { AppStore } from "./app-store";

interface IWhoAmIResult {
    authenticated: boolean;
    realName: string;
    role: string[];
    userName: string;
}

export class UserStore {
    constructor(parentStore: AppStore) {
        this._appStore = parentStore;
    }

    private _appStore: AppStore;

    @observable authenticated: boolean;
    @observable name: string;
    @observable administrator: boolean;

    @action checkAuthentication = async (tryRefresh: boolean = false) => {

        let result = await fetch('/api/account/whoami', {
            credentials: 'same-origin'
        });

        let whoAmI = await result.json() as IWhoAmIResult;

        this.authenticated = whoAmI.authenticated;
        this.name = whoAmI.realName;
        this.administrator = _.includes(whoAmI.role, "AdminUser");

        if (!this.authenticated && tryRefresh)
            await this.refreshAuthentication();
    }

    @action refreshAuthentication = async () => {
        let result = await fetch("/refreshToken", {
            credentials: "same-origin",
            method: "POST"
        });

        await this.checkAuthentication();
    }
}