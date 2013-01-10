import sys
import time
import requests

give_up_at = time.time() + 5 * 60
while time.time() < give_up_at:
	r = requests.get('https://appharbor.com/applications/%s/builds?access_token=%s' % (sys.argv[2], sys.argv[3]), headers={'Accept': 'application/json'})
	results = r.json()
	for result in results:
		if result['commit']['id'] == sys.argv[1]:
			if result['status'] == "Failed":
				sys.exit(1)
			if result['status'] == "Succeeded" and result['deployed'] is not None:
				sys.exit(0)
			break
	time.sleep(20)
sys.exit(2)
