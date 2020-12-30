import React, {PureComponent} from 'react';

import {ESCManager, EventManager, ReducerManager} from "@esc_games/esc-controller-sdk";
import {Router, Route, Switch} from "react-router-dom";

// Phases that your controller responds to
import {
	PHASE_LOBBY,
	PHASE_PLAY,
	PHASE_END,
	Lobby,
	PlayScreen,
	EndScreen,
} from "Lib/phases";

// Events that are fired from the Game
import {
	UC_PHASE_CHANGE,
} from "Main/GameEvents";

// Events that are fired from the Controller, and cause the UI to update
import {
	PREFIX,
	ACTION_START_PLAYING,
} from "Main/UIEvents";

// State reducers that handle actions which are fired within the controller
import {
	spreadReducer,
} from "Lib/reducers";

// The phase router converts phases into browser history
import phaseRouter from "Lib/utils/phaseRouter";
import history from "Lib/utils/history";

// Include the theme CSS
import "Root/theme.css";

// Include the controllers CSS
import "./AggregationExampleAdminController.css";

// The initial state of the application
const initialState = {
	isPlaying: false,
};

// Setup our reducers to handle dispatched UI actions
const reducerManager = new ReducerManager({
	[ACTION_START_PLAYING]: spreadReducer,
}, initialState);

// Create an event manager to handle system events
const AggregationExampleAdminManager = new EventManager('AggregationExampleAdmin', reducerManager);

// Add an event listener for the UC_PHASE_CHANGE event and call the phaseRouter
// This allows the game to drive the controller
ESCManager.networking.registerEventHandler(UC_PHASE_CHANGE, PREFIX, (message) => phaseRouter(message.phaseStats.phase));

class AggregationExampleAdminControllerComponent extends PureComponent {
	handleStartPlaying = () => {
		// Start playing
		AggregationExampleAdminManager.dispatchUI(ACTION_START_PLAYING, {
			isPlaying: true,
		});

		// Move to the Play screen
		phaseRouter(PHASE_PLAY);
	}
	render() {
		return (
			<React.Fragment>
        <strong>This is the AggregationExample controller</strong>

				{/* When not playing, render the lobby phase */}
				{!this.props.isPlaying ? (
					<Lobby onClickPlay={this.handleStartPlaying} />
				) : (
					<Router history={history}>
					{/* When playing, route based on the current phase */}
						<Switch>
							<Route path={`/${PHASE_PLAY}`} render={() => <PlayScreen />} />
							<Route path={`/${PHASE_END}`} render={() => <EndScreen />} />
							<Route render={() => <div>No phase found</div>} />
						</Switch>
					</Router>
				)}
			</React.Fragment>
		);
	}
}

// Export the base controller. This is useful for testing/debugging
export { AggregationExampleAdminControllerComponent };

// Connect controller to the potential UI actions. This causes the controller to respond to the specified actions, and render
export default AggregationExampleAdminManager.connect(AggregationExampleAdminControllerComponent, [
    ACTION_START_PLAYING,
]);
