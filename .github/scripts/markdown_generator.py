import json
import os

lines = ["## 🗂️ File Types (alle Branches)\n"]

results_path = os.path.join('.github', 'results.txt')
output_dir = "Results"
os.makedirs(output_dir, exist_ok=True)

with open(results_path, encoding="utf-8") as f:
    for line in f:
        if not line.strip():
            continue
        data = json.loads(line)
        branch = data["branch"]
        filetypes = data["filetypes"]
        total = sum(filetypes.values())
        
        lines.append(f"### Branch `{branch}`")
        lines.append("| File Type | Count | Percentage |")
        lines.append("|-----------|-------|------------|")
        
        for ext, count in sorted(filetypes.items(), key=lambda x: -x[1]):
            percent = f"{(count/total)*100:.2f}%"  # zwei Nachkommstellen
            bar = "█" * int((count/total)*20)
            lines.append(f"| `{ext}` | {count} | {percent} {bar}|")
        lines.append("")
        
        # JSON für den Branch erstellen
        branch_json = {
            "branch": branch,
            "filetypes": filetypes,
            "total_files": total
        }
        json_path = os.path.join(output_dir, f"Result{branch}.json")
        with open(json_path, "w", encoding="utf-8") as jf:
            json.dump(branch_json, jf, indent=2)

# Markdown-Datei erzeugen
with open("FILETYPES.md", "w", encoding="utf-8") as f:
    f.write("\n".join(lines))
