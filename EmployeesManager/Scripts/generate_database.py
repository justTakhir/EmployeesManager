import requests
import random as rnd

host = input('Введите хост бэкенда (например, http://localhost:5055): ').strip()
count = int(input('Введите количество генерируемых записей: ').strip())

first_names = ['Anna', 'Mikhail', 'Andrew', 'Vladimir', 'Antony', 'Alexander', 'Dmitry', 'Alina', 'Artem', 'Pavel', 'Maxim']
middle_names = ['Alexandrovich', 'Andreevich', 'Sergeevich', 'Artemovich']
last_names = ['Foster', 'Ivanov', 'Rush', 'Hurt', 'Carison', 'Kamaz']
job_titles = ['Programmer', 'Manager', 'QA', 'HR', 'DevOps', 'Team Lead', 'Tech Lead', 'Analyst']

def hire(first_name, middle_name, last_name, job_title, supervisor_id):
	request_string = f'{host}/employees/hire?firstName={first_name}&middleName={middle_name}&lastName={last_name}&jobTitle={job_title}'
	if supervisor_id is not None:
		request_string += f'&supervisorID={supervisor_id}'
	
	return requests.get(request_string).json()


def generate(num_of_employees):
	hire('Maxim', 'Vladimirovich', 'Ishutin', 'Director', None)
	
	for i in range(1, num_of_employees):
		hire(rnd.choice(first_names), rnd.choice(middle_names), rnd.choice(last_names), rnd.choice(job_titles), rnd.randint(1, i))
	
generate(count)