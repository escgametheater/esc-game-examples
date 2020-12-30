import React from "react";

import "./PlayScreen.css";
const aggButt = require("./images/aggButt.png");

const PlayScreen = ({onClickAgg}) => (
	<div className="play-screen">
		<div className="button-wrapper">
			<button style={{ backgroundImage: `url(${aggButt})` }}
					onClick={(e) =>
					{e.preventDefault();
						onClickAgg();
						console.log("on Click");
					}}>
			</button></div>
	</div>
);

export default PlayScreen;