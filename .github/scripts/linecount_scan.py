import os
import json
import subprocess

# Ergebnisse in All_Code/
output_dir = "All_Code"
os.makedirs(output_dir, exist_ok=True)

# Aktuellen Branch herausfinden
branch = subprocess.check_output(
    ["git", "rev-parse", "--abbrev-ref", "HEAD"]
).decode().strip()

# Dateiendungen, die wir zählen wollen
count_exts = {".cs", ".py", ".html", ".css", ".js"}

def count_lines(filepath):
    try:
        with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
            return sum(1 for _ in f)
    except:
        return 0

line_counter = {}
total_lines = 0

for root, dirs, files in os.walk("."):
    if ".git" in root or ".github" in root:
        continue
    for f in files:
        ext = os.path.splitext(f)[1].lower()
        if ext in count_exts:
            path = os.path.join(root, f)
            lines = count_lines(path)
            total_lines += lines
            line_counter[ext] = line_counter.get(ext, 0) + lines

result = {
    "branch": branch,
    "total_lines": total_lines,
    "lines_by_ext": line_counter,
}

json_path = os.path.join(output_dir, f"Lines_{branch}.json")
with open(json_path, "w", encoding="utf-8") as jf:
    json.dump(result, jf, indent=2)

print(f"Branch '{branch}' gescannt. Zeilen gespeichert unter {json_path}")
