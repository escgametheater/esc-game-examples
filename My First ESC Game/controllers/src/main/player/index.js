import React from 'react';
import ReactDOM from 'react-dom';
import { ESCGameController } from '@esc_games/esc-controller-sdk/';
import MyPlayerController from './MyPlayerController';

ReactDOM.render(
	<ESCGameController 
		game="My"
		includeESCLogo={false}
		includeTattooDisplay={false}
		includeTattooSelector={false}
		desiredOrientation={"portrait"}
		autoSizeEnabled={true}
		appModeEnabled={true}
		nosleep={true}
	>
		<MyPlayerController />
	</ESCGameController>,
	document.getElementById('root')
);

if (process.env.NODE_ENV === "development") {
  module.hot.accept();
}
