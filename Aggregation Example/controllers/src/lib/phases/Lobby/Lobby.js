import React from "react";

import "./Lobby.css";

const Lobby = ({onClickPlay}) => (
	<div className="lobby">
		Lobby!
		<button onClick={(e) => {
			e.preventDefault();
			onClickPlay();
		}}>Play</button>
	</div>
);

export default Lobby;