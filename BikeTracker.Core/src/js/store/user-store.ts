import { observable, action } from 'mobx';

interface IWhoAmIResult {
    authenticated: boolean;
    realName: string;
    role: string[];
    userName: string;
}

export class UserStore {
    @observable authenticated: boolean;
    @observable name: string;

    @action checkAuthentication = async (tryRefresh:boolean = false) => {
        let result = await fetch('/api/account/whoami', {
            credentials: 'same-origin'
        });

        let whoAmI = await result.json() as IWhoAmIResult;

        this.authenticated = whoAmI.authenticated;
        this.name = whoAmI.realName;

        if (!this.authenticated && tryRefresh)
            this.refreshAuthentication();
    }

    @action refreshAuthentication = async () => {
        let result = await fetch("/refreshToken", {
            credentials: "same-origin",
            method: "POST"
        });

        await this.checkAuthentication();
    }
}