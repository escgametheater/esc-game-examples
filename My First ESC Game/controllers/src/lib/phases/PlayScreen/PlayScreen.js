import React from "react";

import "./PlayScreen.css";

const PlayScreen = ({player, onButtonClick,}) => (
		<div className="play-screen">
			
			<button onClick={(e) => {
				e.preventDefault();
				onButtonClick();
			}}> {player.PlayerName}
			</button>
		</div>
);

export default PlayScreen;