import json

lines = ["## 🗂️ File Types (alle Branches)\n"]

with open("results.txt", encoding="utf-8") as f:
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
            percent = f"{(count/total)*100:.1f}%"
            bar = "█" * int((count/total)*20)
            lines.append(f"| `{ext}` | {count} | {percent} {bar}|")
        lines.append("")

with open("FILETYPES.md", "w", encoding="utf-8") as f:
    f.write("\n".join(lines))