import { UserStore } from "./user-store";
import { observable, action } from 'mobx';

export class AppStore {
    @observable 
    user: UserStore = new UserStore();
    @observable 
    error: string = "";

    @action showError = (msg: string) => {
        this.error = msg;
    }

    @action clearError = () => {
        this.error = "";
    }
}