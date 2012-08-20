function sleep(milliSeconds,callback){
		var startTime = new Date().getTime();
		while(new Date().getTime()<startTime+milliSeconds);
		callback();
	}

exports.sleep=sleep;