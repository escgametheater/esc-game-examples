import history from "./history";

/**
 * Converts a phase into History
 */
const phaseRouter = (phase) => {
	const location = {
		pathname: "/" + phase,
	};

	history.replace(location);
}

export default phaseRouter;
