import os
import json
import subprocess
from collections import Counter

# Ordner für Ergebnisse
output_dir = "Results"
os.makedirs(output_dir, exist_ok=True)

# Branches, die wir prüfen wollen
branches_to_scan = ["main", "Wiki"]

# Alle Remote-Branches abrufen
remote_branches = subprocess.check_output(['git', 'branch', '-r']).decode().splitlines()
remote_branches = [b.strip().replace('origin/', '') for b in remote_branches if 'HEAD' not in b]

for branch in branches_to_scan:
    if branch not in remote_branches:
        print(f"Branch '{branch}' existiert nicht im Remote, überspringe.")
        continue

    # Branch lokal erstellen oder wechseln
    subprocess.run(['git', 'checkout', '-B', branch, f'origin/{branch}'],
                   stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)

    # Dateien sammeln
    filetypes = []
    for root, dirs, files in os.walk('.'):
        if '.git' in root or '.github' in root:
            continue
        for f in files:
            if '.' in f:
                ext = f[f.rfind('.'):]
                filetypes.append(ext.lower())
            else:
                filetypes.append('(no ext)')

    if not filetypes:
        filetypes.append('(no files found)')

    counter = Counter(filetypes)
    result = {
        "branch": branch,
        "filetypes": dict(counter),
        "total_files": sum(counter.values())
    }

    # JSON speichern
    json_path = os.path.join(output_dir, f"Result{branch}.json")
    with open(json_path, "w", encoding="utf-8") as jf:
        json.dump(result, jf, indent=2)
