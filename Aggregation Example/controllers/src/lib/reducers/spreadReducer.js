const spreadReducer = (state, action) => {
	console.log('Spread Reducer', state, action);
	
	// Merge our current state, with the new state
	return {
		...state,
		...action.value,
	};
};

export default spreadReducer;