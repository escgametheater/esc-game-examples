import React from "react";
import Lobby from "./Lobby";
import PlayScreen from "./PlayScreen";
import EndScreen from "./EndScreen";

const PHASE_LOBBY = "Lobby";
const PHASE_PLAY = "Play";
const PHASE_END = "End";

export {
	/* Phases */
	PHASE_LOBBY,
	PHASE_PLAY,
	PHASE_END,

	/* Phase components */
	Lobby,
	PlayScreen,
	EndScreen,
};