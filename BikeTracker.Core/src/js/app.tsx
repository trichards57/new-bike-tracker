import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Root } from './root';
import { AppStore } from "./store/app-store";
import { Provider } from "mobx-react";
import DevTools from "mobx-react-devtools";

const appStore = new AppStore();

ReactDOM.render(
    <div>
        <Provider appStore={appStore}>
            <Root store={appStore} />
        </Provider>
        <DevTools />
    </div>,
    document.getElementById('root'));
